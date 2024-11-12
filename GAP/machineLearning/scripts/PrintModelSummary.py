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

import tensorflow.keras as keras

MODEL_SRC = r"..\..\..\machinelearning\ladybug\cnn_model.keras"

model = keras.models.load_model(MODEL_SRC)

model.summary()

# this line doesn't work for an unknown reason
#keras.utils.plot_model(model, "model.png")