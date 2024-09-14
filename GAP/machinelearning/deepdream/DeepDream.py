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

PARAMS_LEN = 8

model = inception_v3.InceptionV3(weights = "imagenet", include_top = False)

layer_settings = {
    "mixed4": 1.0,
    "mixed5": 1.5,
    "mixed6": 2.0,
    "mixed7": 2.5,
}

outputs_dict = dict(
    [
        (layer.name, layer.output)
        for layer in [model.get_layer(name) for name in layer_settings.keys()]
    ]
)

feature_extractor = keras.Model(inputs = model.inputs, outputs = outputs_dict)

def read_params_file():
    file = open(r"..\..\..\machinelearning\deepdream\params.txt")
    content = file.read()
    file.close()
    content_arr = content.split("\n")
    assert len(content_arr) == PARAMS_LEN
    return content.split("\n")

def import_params():
    global IMG_NAME, IMG_ORIGIN, VERBOSE, LEARNING_RATE, NUM_OCTAVE, OCTAVE_SCALE, ITERATIONS, MAX_LOSS
    params = read_params_file()

    IMG_NAME = "coast.jpg"
    IMG_ORIGIN = "https://img-datasets.s3.amazonaws.com/coast.jpg"

    VERBOSE = params[2] == "True"
    LEARNING_RATE = float(params[3])
    NUM_OCTAVE = int(params[4])
    OCTAVE_SCALE = float(params[5])
    ITERATIONS = int(params[6])
    MAX_LOSS = float(params[7])

def get_file(name, origin):
    return keras.utils.get_file(name, origin = origin)

def get_img_shape(name, origin):
    base_image_path = get_file(name, origin)
    o_img = preprocess_image(base_image_path)
    o_shape = o_img.shape[1:3]
    return o_img, o_shape

def preprocess_image(image_path):
    img = keras.utils.load_img(image_path)
    img = keras.utils.img_to_array(img)
    img = np.expand_dims(img, axis = 0)
    img = keras.applications.inception_v3.preprocess_input(img)
    return img

def deprocess_image(img):
    img = img.reshape((img.shape[1], img.shape[2], 3))
    img /= 2.0
    img += 0.5
    img *= 255.0
    img = np.clip(img, 0, 255).astype("uint8")
    return img

def calculate_consecutive_shapes(original):
    shapes = [original]
    for i in range(1, NUM_OCTAVE):
        shape = tuple([int(dim / (OCTAVE_SCALE ** i)) for dim in original])
        shapes.append(shape)
    return shapes[::-1]

def compute_loss(input_image):
    features = feature_extractor(input_image)
    loss = tf.zeros(shape=())

    for name in features.keys():
        coefficient = layer_settings[name]
        activation = features[name]

        loss += coefficient * tf.reduce_mean(tf.square(activation[:, 2:-2, 2:-2, :]))
    return loss

@tf.function
def gradient_ascent_step(image, learning_rate):
    with tf.GradientTape() as tape:
        tape.watch(image)
        loss = compute_loss(image)

    gradients = tape.gradient(loss, image)
    gradients = tf.math.l2_normalize(gradients)
    image += learning_rate * gradients
    return loss, image

def gradient_ascent_loop(image, iterations, learning_rate, max_loss):
    for i in range(iterations):
        loss, image = gradient_ascent_step(image, learning_rate)
        if max_loss is not None and loss > max_loss:
            break

        if VERBOSE:
            print(f"loss value at step {i + 1}: {loss:.2f}")
    return image

def loop_octaves(original_img, original_shape):
    consecutive_shapes = calculate_consecutive_shapes(original_shape)
    shrunk_original_img = tf.image.resize(original_img, consecutive_shapes[0])
    img = tf.identity(original_img)

    for i, shape in enumerate(consecutive_shapes):
        if VERBOSE:
            print(f"processing octave {i + 1} with shape {shape}")

        img = tf.image.resize(img, shape)
        img = gradient_ascent_loop(img, iterations = ITERATIONS, learning_rate = LEARNING_RATE, max_loss = MAX_LOSS)

        upscaled_shrunk_original_img = tf.image.resize(shrunk_original_img, shape)
        same_size_original = tf.image.resize(original_img, shape)
        lost_detail = same_size_original - upscaled_shrunk_original_img
        img += lost_detail
        shrunk_original_img = tf.image.resize(original_img, shape)
    return img

def generate(img_name, img_origin):
    o_img, o_shape = get_img_shape(img_name, img_origin)
    return loop_octaves(o_img, o_shape)

import_params()

output_img = generate(IMG_NAME, IMG_ORIGIN)

keras.utils.save_img(r"..\..\..\machinelearning\deepdream\output\dream.png", deprocess_image(output_img.numpy()))