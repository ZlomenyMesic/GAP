#
#
#
#
#          THIS IS JUST A TESTING PROGRAM
#
#
#
#

# disable annoying warnings
import os
os.environ['TF_ENABLE_ONEDNN_OPTS'] = '0'
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

from tensorflow import keras
from tensorflow.keras import layers
from tensorflow.keras.datasets import mnist
import matplotlib.pyplot as plt

EPOCHS = 1
BATCH_SIZE = 128

# test digit index
TEST_INDEX = 1

# load the MNIST database of hand-written digits
# includes 60 000 digits for training and 10 000 digits for testing
(train_images, train_labels), (test_images, test_labels) = mnist.load_data()

# create a 2-layer neural network
model = keras.Sequential([
    layers.Dense(512, activation = "relu"),
    layers.Dense(10, activation = "softmax")
])

model.compile(optimizer = "rmsprop",
              loss = "sparse_categorical_crossentropy",
              metrics = ["accuracy"]
)

# draw the tested digit
digit = test_images[TEST_INDEX]
plt.imshow(digit, cmap=plt.cm.binary)
plt.show()

# reshape the dataset into a more practical format
train_images = train_images.reshape((60_000, 28 * 28))
train_images = train_images.astype("float32") / 255

test_images = test_images.reshape((10_000, 28 * 28))
test_images = test_images.astype("float32") / 255

# train the model
model.fit(train_images, train_labels, epochs = EPOCHS, batch_size = BATCH_SIZE, verbose = 0)


# test the model with a selected testing digit
test_digit = test_images[TEST_INDEX:(TEST_INDEX + 1)]
predictions = model.predict(test_digit, verbose = 0)

guess = predictions[0].argmax()
confidence = predictions[0][guess]
answer = test_labels[TEST_INDEX]
correct = guess == answer

# print the result
print("prediction:", guess,
      "\nconfidence:", confidence * 100, "%",
      "\ncorrect label:", test_labels[TEST_INDEX],
      "\nguessed correctly:", correct
)