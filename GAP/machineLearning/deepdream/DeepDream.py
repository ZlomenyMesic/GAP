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

import sys, os
sys.path.append(os.path.relpath(r"..\..\..\machinelearning\utils"))
from ConsoleColors import *

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
    global VERBOSE, IMG_NAME, IMG_ORIGIN, IMG_ORIGIN_FORMAT, OUTPUT_PATH, DONE, DISTORTION_RATE, OCTAVES, OCT_SCALE, ITERATIONS, MAX_LOSS, LAYERS, LAYER_ACTIVATIONS
    params = read_params_file()

    VERBOSE = params[0] == "True"

    IMG_NAME = params[1]
    IMG_ORIGIN = params[2]
    IMG_ORIGIN_FORMAT = int(params[3])
    OUTPUT_PATH = params[4]
    DONE = params[5]

    DISTORTION_RATE = float(params[6])
    OCTAVES = int(params[7])
    OCT_SCALE = float(params[8])
    ITERATIONS = int(params[9])
    MAX_LOSS = float(params[10])

    LAYERS = params[11].split(" ")
    LAYER_ACTIVATIONS = params[12].split(" ")

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


# doesn't really calculate loss, but rather layer activation
def calculate_loss(input_image):
    # the extracted layer
    # this processes the input image with the given layers
    features = feature_extractor(input_image)

    loss = tf.zeros(shape=())

    # loop through the extracted features (in our case just a single layer)
    for name in features.keys():
        # coefficient - how much should it affect the image
        coefficient = layer_settings[name]

        # activations of the current extracted layer
        activation = features[name]

        # mean squared activations
        mean_sq_act = tf.reduce_mean(tf.square(activation))

        # update the loss value
        loss += coefficient * mean_sq_act
    return loss

# as opposed to most neural networks, we actually try to maximize the 
# loss function instead of minimizing it. this is called gradient ascent
def gradient_ascent_step(image, distortion_rate):
    # GradientTape record all tensor operations made with the image.
    # it is a very useful tool for automatic differentiation
    with tf.GradientTape() as tape:
        tape.watch(image)
        loss = calculate_loss(image)

    # retroactively calculate the loss gradient with respect to the different parts of the image
    gradients = tape.gradient(loss, image)

    # without normalizing the gradients, layers with lower activations would become unnoticeable
    gradients = tf.math.l2_normalize(gradients)

    # update the image using the calculated gradients
    image += distortion_rate * gradients
    return loss, image

# loop repeats NUM_OCTAVE times with ITERATIONS steps
def gradient_ascent_loop(image, iterations, distortion_rate, max_loss):
    for i in range(iterations):
        loss, image = gradient_ascent_step(image, distortion_rate)

        # break the loop when maximum allowed loss is met
        if max_loss is not None and loss >= max_loss:
            break
    return image

# this is the main loop. the idea is that first smaller and smaller
# versions of the original image are calculated and then consecutively
# put together. this means that in the first octave the image starts
# on the lowest resolution and progressively gets larger each octave
# until reaching the original size on the last octave. during each 
# octave, the "dream effect" also gets applied. the reason for changing
# the resolution is that the dream patterns will not be the same size.
def loop_octaves(original_img, original_shape):
    # calculate the consecutive sizes going from the smallest to largest
    consecutive_shapes = calculate_consecutive_shapes(original_shape)

    # create a copy of the original image with the initial (smallest) resolution
    shrunk_original_img = tf.image.resize(original_img, consecutive_shapes[0])

    # clone the original image
    img = tf.identity(original_img)

    for i, shape in enumerate(consecutive_shapes):
        if VERBOSE:
            print(f"   Processing octave {YELLOW}{i + 1}/{OCTAVES}{WHITE} with shape {shape}    ", end = '\r')

        # resize the image
        img = tf.image.resize(img, shape)

        # apply the "dream effect"
        img = gradient_ascent_loop(img, iterations = ITERATIONS, distortion_rate = DISTORTION_RATE, max_loss = MAX_LOSS)

        # 1. upscale the original image clone without any effect added
        # 2. downscale the original image to the same size
        # 3. calculate the difference between the two same sized images
        # 4. add the lost detail back to the image with the dream effect
        # 5. resize the shrunk clone to another size for the next octave
        upscaled_shrunk_original_img = tf.image.resize(shrunk_original_img, shape)
        same_size_original = tf.image.resize(original_img, shape)
        lost_detail = same_size_original - upscaled_shrunk_original_img
        img += lost_detail
        shrunk_original_img = tf.image.resize(original_img, shape)

    if VERBOSE:
        print(f"   Finished octave {YELLOW}{OCTAVES}/{OCTAVES}{WHITE}                                       ")

    return img

# creates the feature extractor for each layer
def extract_layer(name):
    # feature extraction is a process during which an intermediate output
    # of a given layer is extracted. this means stopping the model in one
    # of its hidden layers and taking the current result without continuing
    # the forward pass. outputs_dict assignes each layer to its extracted
    # features. in our case we only extract a single layer at a time.
    outputs_dict = {
        name : model.get_layer(name).output
    }

    # create a new extraction model starting with the same input as the
    # original model, but ending in the required extraction layer
    global feature_extractor
    feature_extractor = keras.Model(inputs = model.inputs, outputs = outputs_dict)

def create_layer_dict():
    global layer_settings
    layer_settings = dict()

    for i in range(len(LAYERS)):
        layer_settings[LAYERS[i]] = int(LAYER_ACTIVATIONS[i])

def loop_layers():
    global CUR_LAYER, CUR_LAYER_ACTIVATION

    img, shape = get_img_shape(IMG_NAME, IMG_ORIGIN, IMG_ORIGIN_FORMAT)

    i = 0
    for layer in layer_settings.keys():
        CUR_LAYER = layer
        CUR_LAYER_ACTIVATION = layer_settings[layer]

        if VERBOSE:
            print(f"{CYAN}Layer {i + 1}{WHITE} - Name: {GREEN}{CUR_LAYER}{WHITE}; Activation: {CUR_LAYER_ACTIVATION}")

        extract_layer(layer)

        output = loop_octaves(img, shape)

        # save the current image into output folder, if this isn't the' last iteration,
        # the saved image will be reused as the input for the next cycle (layer)
        keras.utils.save_img(OUTPUT_PATH, deprocess_image(output.numpy()))
        if (i != len(LAYERS) - 1):
            img, shape = get_img_shape(IMG_NAME, OUTPUT_PATH, 0)

        if VERBOSE:
            print("")

        i += 1
    return img

import_params()

if VERBOSE:
    print(f"Running DeepDream:")
    print(f"   {GREEN}{CIRCLE}{WHITE} Input source: {IMG_ORIGIN}")
    print(f"   {GREEN}{CIRCLE}{WHITE} Output path: {OUTPUT_PATH}")
    print(f"   {GREEN}{CIRCLE}{WHITE} Layers to iterate: {RED}{len(LAYERS)}{WHITE}\n")

create_layer_dict()

# neural network model pretrained on imagenet database
# pretrained weights save time and work, training out own network would be too difficult
model = inception_v3.InceptionV3(weights = "imagenet", include_top = False)

#print([layer.name for layer in model.layers])

# run the generator
output_img = loop_layers()

# create a file named after the current iteration number
# this lets the main C# program know that the work here is done
f = open(DONE, "w")
f.close()

if VERBOSE:
    print(f"Output image successfully saved to: {OUTPUT_PATH}")