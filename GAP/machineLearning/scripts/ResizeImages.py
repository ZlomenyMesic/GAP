#
# GAP - Generative Art Producer
#   by ZlomenyMesic & KryKom
#
#      founded 11.9.2024
#

# THIS SCRIPT IS USED TO RESIZE ALL IMAGES FROM A DIRECTORY INTO A UNIFORM SIZE

from PIL import Image
import os

SOURCE = "dataset"
OUTPUT = "resized"
SIZE = (280, 210)

def resize(path, name):
    image = Image.open(path)
    image = image.resize(SIZE)
    image.save(fr"{OUTPUT}\{name}")

# recursion allows searching in subfolders
def recursive_search(dir):
    for item in os.listdir(dir):
        path = fr"{dir}\{item}"
        if os.path.isfile(path):
            resize(path, item)
        else:
            recursive_search(path)

recursive_search(SOURCE)

print("done")
input()