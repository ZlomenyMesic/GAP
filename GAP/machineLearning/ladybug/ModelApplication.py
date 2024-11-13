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
import numpy as np

import sys, os
sys.path.append(os.path.relpath(r"..\..\..\machinelearning\utils"))
from ConsoleColors import *

MODEL_PATH = r"..\..\..\machinelearning\ladybug"
MODEL_NAME = "cnn_model.keras"
OUTPUT_PATH = r"..\..\..\machinelearning\ladybug\output\output.png"

ITERATIONS = 100
DISTORTION_RATE = 15

def random_noise():
    noise = np.random.random((1, 180, 180, 3))
    return tf.convert_to_tensor(noise)

def preprocess_image(image_path):
    img = keras.utils.load_img(image_path)
    img = keras.utils.img_to_array(img)
    img = np.expand_dims(img, axis = 0)
    return tf.convert_to_tensor(img)

def save_image(img):
    img = img.reshape((img.shape[1], img.shape[2], 3))
    img /= 2.0
    img += 0.5
    img *= 255.0
    img = np.clip(img, 0, 255).astype("uint8")
    keras.utils.save_img(OUTPUT_PATH, img)

def calculate_loss(image):
    activation = model(image)

    loss = tf.zeros(shape=())
    loss += tf.reduce_mean(activation)
    return loss, activation

def gradient_ascent_step(image, distortion_rate, i):
    # GradientTape record all tensor operations made with the image.
    # it is a very useful tool for automatic differentiation
    with tf.GradientTape() as tape:
        tape.watch(image)
        loss, activation = calculate_loss(image)

    # retroactively calculate the loss gradient with respect to the different parts of the image
    gradients = tape.gradient(loss, image)

    # without normalizing the gradients, layers with lower activations would become unnoticeable
    gradients = tf.math.l2_normalize(gradients)

    # update the image using the calculated gradients
    image += distortion_rate * -gradients
    return loss, activation, image

# loop repeats NUM_OCTAVE times with ITERATIONS steps
def gradient_ascent_loop(image):
    for i in range(ITERATIONS):
        loss, activation, image = gradient_ascent_step(image, DISTORTION_RATE, i)
        print(f"{CYAN}Iteration {i + 1}/{ITERATIONS}{WHITE} - Loss: {loss:.5f}, Activation: {activation[0][0]:.5f}", end = "\r")
    return image

print(f"Running Ladybug:")
print(f"   {GREEN}{CIRCLE}{WHITE} Output path: {OUTPUT_PATH}")
print(f"   {GREEN}{CIRCLE}{WHITE} Total iterations: {RED}{ITERATIONS}{WHITE}")
print(f"   {GREEN}{CIRCLE}{WHITE} Distortion rate:  {RED}{DISTORTION_RATE}{WHITE}\n")

model = keras.models.load_model(fr"{MODEL_PATH}\{MODEL_NAME}")
print(f"Model {MAGENTA}{MODEL_NAME}{WHITE} loaded successfully.\n");

noise = random_noise()
output = gradient_ascent_loop(noise)

save_image(output.numpy())
print(f"\n\nOutput saved to: {YELLOW}{OUTPUT_PATH}{WHITE}\n")

f = open("DONE", "w")
f.close()