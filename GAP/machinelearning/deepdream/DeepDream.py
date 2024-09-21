#
# GAP - Generative Art Producer
#   by ZlomenyMesic & KryKom
#
#      founded 11.9.2024
#

# disable annoying warnings
import os
os.environ['TF_ENABLE_ONEDNN_OPTS'] = '0'
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

import tensorflow as tf
import tensorflow.keras as keras
import tensorflow.keras.applications.inception_v3 as inception_v3
import matplotlib.pyplot as plt
import numpy as np

# number of parameters passed in params.txt
PARAMS_LEN = 13

# read the content of params.txt and split it into separate lines
# each line represents a single parameter
def read_params_file():
    file = open(r"..\..\..\machinelearning\deepdream\params.txt")
    content = file.read()
    file.close()
    content_arr = content.split("\n")
    assert len(content_arr) == PARAMS_LEN
    return content.split("\n")

# update the local variables depending on the imported parameters
def import_params():
    global IMG_NAME, IMG_ORIGIN, IMG_ORIGIN_FORMAT, OUTPUT_PATH, VERBOSE, LEARNING_RATE, OCTAVES, OCT_SCALE, ITERATIONS, MAX_LOSS, CUR_LAYER, CUR_LAYER_ACTIVATION, ITER_NUM
    params = read_params_file()

    IMG_NAME = params[0]
    IMG_ORIGIN = params[1]
    IMG_ORIGIN_FORMAT = int(params[2])
    OUTPUT_PATH = params[3]

    VERBOSE = params[4] == "True"
    LEARNING_RATE = float(params[5])
    OCTAVES = int(params[6])
    OCT_SCALE = float(params[7])
    ITERATIONS = int(params[8])
    MAX_LOSS = float(params[9])

    CUR_LAYER = params[10]
    CUR_LAYER_ACTIVATION = int(params[11])
    ITER_NUM = int(params[12])

# used to download the image
def get_url_file(name, origin):
    return keras.utils.get_file(name, origin = origin)

# returns the image and its shape
# origin format: 0 = file path, 1 = url
def get_img_shape(name, origin, format):
    img_path = origin
    if format == 1:
        img_path = get_url_file(name, origin)

    o_img = preprocess_image(img_path)
    o_shape = o_img.shape[1:3]
    return o_img, o_shape

# preprocess the image into a form compatible with the network
def preprocess_image(image_path):
    img = keras.utils.load_img(image_path)
    img = keras.utils.img_to_array(img)
    img = np.expand_dims(img, axis = 0)
    img = keras.applications.inception_v3.preprocess_input(img)
    return img

# reversed function above
def deprocess_image(img):
    img = img.reshape((img.shape[1], img.shape[2], 3))
    img /= 2.0
    img += 0.5
    img *= 255.0
    img = np.clip(img, 0, 255).astype("uint8")
    return img

# calculate the sequence of progresively larger image resolutions
# (during each octave, the image resolution is multiplied by OCTAVE_SCALE)
def calculate_consecutive_shapes(original):
    shapes = [original]
    for i in range(1, OCTAVES):
        shape = tuple([int(dim / (OCT_SCALE ** i)) for dim in original])
        shapes.append(shape)
    return shapes[::-1]

# loss function
def calculate_loss(input_image):
    features = feature_extractor(input_image)
    loss = tf.zeros(shape=())

    for name in features.keys():
        coefficient = layer_settings[name]
        activation = features[name]

        loss += coefficient * tf.reduce_mean(tf.square(activation[:, 2:-2, 2:-2, :]))
    return loss

# single gradient ascent step
# the goal is to maximize the loss function
@tf.function
def gradient_ascent_step(image, learning_rate):
    with tf.GradientTape() as tape:
        tape.watch(image)
        loss = calculate_loss(image)

    gradients = tape.gradient(loss, image)
    gradients = tf.math.l2_normalize(gradients)
    image += learning_rate * gradients
    return loss, image

# loop repeats NUM_OCTAVE times with ITERATIONS steps
def gradient_ascent_loop(image, iterations, learning_rate, max_loss):
    for i in range(iterations):
        loss, image = gradient_ascent_step(image, learning_rate)
        if max_loss is not None and loss > max_loss:
            break
    return image

# the main cycle, loops NUM_OCTAVE times
# image resolution gets gradually bigger and "dream effect" gets applied using gradient ascent loop
def loop_octaves(original_img, original_shape):
    consecutive_shapes = calculate_consecutive_shapes(original_shape)
    shrunk_original_img = tf.image.resize(original_img, consecutive_shapes[0])
    img = tf.identity(original_img)

    for i, shape in enumerate(consecutive_shapes):
        if VERBOSE:
            print(f"   octave {i + 1}/{OCTAVES} with shape {shape}")

        # resize the image and apply "dream effect"
        img = tf.image.resize(img, shape)
        img = gradient_ascent_loop(img, iterations = ITERATIONS, learning_rate = LEARNING_RATE, max_loss = MAX_LOSS)

        # puts back details lost during the resolution change
        upscaled_shrunk_original_img = tf.image.resize(shrunk_original_img, shape)
        same_size_original = tf.image.resize(original_img, shape)
        lost_detail = same_size_original - upscaled_shrunk_original_img
        img += lost_detail
        shrunk_original_img = tf.image.resize(original_img, shape)
    return img

# wrapper function
def generate(img_name, img_origin, img_origin_format):
    o_img, o_shape = get_img_shape(img_name, img_origin, img_origin_format)
    return loop_octaves(o_img, o_shape)

import_params()

# neural network model pretrained on imagenet database
# used to save time and work, training a network ourselves would be inefficient
model = inception_v3.InceptionV3(weights = "imagenet", include_top = False)

#print([layer.name for layer in model.layers])

# activations of different layers
layer_settings = {
    CUR_LAYER: CUR_LAYER_ACTIVATION
}
if VERBOSE:
    print(f"iteration: {ITER_NUM}, layer name: {CUR_LAYER}, layer activation: {CUR_LAYER_ACTIVATION}")

outputs_dict = dict(
    [
        (layer.name, layer.output)
        for layer in [model.get_layer(name) for name in layer_settings.keys()]
    ]
)

feature_extractor = keras.Model(inputs = model.inputs, outputs = outputs_dict)

output_img = generate(IMG_NAME, IMG_ORIGIN, IMG_ORIGIN_FORMAT)

# save the output image
keras.utils.save_img(fr"..\..\..\machinelearning\deepdream\output\dream.png", deprocess_image(output_img.numpy()))

# create a file named after the current iteration number
# this lets the main C# program know that the work here is done
f = open(fr"..\..\..\machinelearning\deepdream\output\{ITER_NUM}", "w")
f.close()


if VERBOSE:
    print("\n")