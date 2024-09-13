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
from tensorflow import keras
from tensorflow.keras.applications import inception_v3
import matplotlib.pyplot as plt
import numpy as np

VERBOSE = True
LEARNING_RATE = 20.0
NUM_OCTAVE = 3
OCTAVE_SCALE = 1.4
ITERATIONS = 30
MAX_LOSS = None

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

def get_file(name, origin):
    return keras.utils.get_file(name, origin = origin)

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
        shape = tuple([int(dim / (OCTAVE_SCALE ** i)) for dim in original_shape])
        shapes.append(shape)
    return shapes[::-1]

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


base_image_path = get_file("coast.jpg", "https://img-datasets.s3.amazonaws.com/coast.jpg")

original_img = preprocess_image(base_image_path)
original_shape = original_img.shape[1:3]

img = loop_octaves(original_img, original_shape)

keras.utils.save_img(r"..\..\..\machinelearning\deepdream\output\dream.png", deprocess_image(img.numpy()))