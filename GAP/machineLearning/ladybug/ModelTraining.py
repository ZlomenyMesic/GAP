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

import pathlib
from tensorflow import keras
from tensorflow.keras import layers
from tensorflow.keras.utils import image_dataset_from_directory

import sys, os
sys.path.append(os.path.relpath(r"..\..\..\machinelearning\utils"))
from ConsoleColors import *

DATASET_DIR = pathlib.Path(r"C:\Users\michn\Desktop\catsndawgs\train_modif")
MODEL_PATH = r"..\..\..\machinelearning\ladybug"
MODEL_NAME = "cnn_model.keras"

IMG_SIZE = 180
BATCH_SIZE = 32
EPOCHS = 3

# expects the subsets already exist
# if they don't, create them using DataSubsets.py
def load_subsets():
    global train_dataset, validation_dataset, test_dataset
    
    train_dataset = image_dataset_from_directory(DATASET_DIR / "train", image_size = (IMG_SIZE, IMG_SIZE), batch_size = BATCH_SIZE)
    print(f"\033[F\033[44G ... subset {GREEN}train{WHITE} loaded correctly")

    validation_dataset = image_dataset_from_directory(DATASET_DIR / "validation", image_size = (IMG_SIZE, IMG_SIZE), batch_size = BATCH_SIZE)
    print(f"\033[F\033[44G ... subset {GREEN}validation{WHITE} loaded correctly")

    test_dataset = image_dataset_from_directory(DATASET_DIR / "test", image_size = (IMG_SIZE, IMG_SIZE), batch_size = BATCH_SIZE)
    print(f"\033[F\033[44G ... subset {GREEN}test{WHITE} loaded correctly\n")

def create_model():
    input = keras.Input(shape = (IMG_SIZE, IMG_SIZE, 3))

    # data augmentation part
    x = layers.RandomFlip("horizontal")(input)
    x = layers.RandomRotation(0.1)(x)
    x = layers.RandomZoom(0.2)(x)
    x = layers.Rescaling(1.0 / 255)(x)

    # convolutional base part
    x = layers.Conv2D(filters = 32, kernel_size = 3, activation = "relu")(x)
    x = layers.MaxPooling2D(pool_size = 2)(x)

    x = layers.Conv2D(filters = 64, kernel_size = 3, activation = "relu")(x)
    x = layers.MaxPooling2D(pool_size = 2)(x)

    x = layers.Conv2D(filters = 128, kernel_size = 3, activation = "relu")(x)
    x = layers.MaxPooling2D(pool_size = 2)(x)

    x = layers.Conv2D(filters = 256, kernel_size = 3, activation = "relu")(x)
    x = layers.MaxPooling2D(pool_size = 2)(x)

    x = layers.Conv2D(filters = 256, kernel_size = 3, activation = "relu")(x)

    # densely connected classificator part
    x = layers.Flatten()(x)
    x = layers.Dropout(0.5)(x)

    output = layers.Dense(1, activation = "sigmoid")(x)
    return keras.Model(inputs = input, outputs = output)

def train_model():
   return model.fit(train_dataset, epochs = EPOCHS, validation_data = validation_dataset, callbacks = callbacks)

print(f"\nStarting Ladybug training cycle:")
print(f"   {GREEN}{CIRCLE}{WHITE} Epochs: {RED}{EPOCHS}{WHITE}")
print(f"   {GREEN}{CIRCLE}{WHITE} Batch size: {RED}{BATCH_SIZE}{WHITE}\n")

load_subsets()

model = create_model()

model.compile(loss = "binary_crossentropy", optimizer = "rmsprop", metrics = ["accuracy"])

# auto-save for the model
callbacks = [
    keras.callbacks.ModelCheckpoint(
        filepath = fr"{MODEL_PATH}\{MODEL_NAME}",

        # only save the best version of the model (don't overwrite an older but better one)
        # how good a model is is measured using the validation loss
        save_best_only = True,
        monitor = "val_loss"
)]

# training data is saved to history in case someone wants to view it
history = train_model()

test_loss, test_acc = model.evaluate(test_dataset)
print("\nTraining finished:")
print(f"   {GREEN}{CIRCLE}{WHITE} Testing loss: {CYAN}{test_loss:.3f}{WHITE}")
print(f"   {GREEN}{CIRCLE}{WHITE} Testing accuracy: {CYAN}{test_acc:.3f}{WHITE}\n")

# load the best version of the model and evaluate it on the testing subset
#test_model = keras.models.load_model(MODEL_NAME)
#test_loss, test_acc = test_model.evaluate(test_dataset)
#print(f"test loss: {test_loss:.3f}\ntest accuracy: {test_acc:.3f}")

f = open(r"DONE", "w")
f.close()