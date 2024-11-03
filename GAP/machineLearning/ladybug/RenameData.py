#
# GAP - Generative Art Producer
#   by ZlomenyMesic & KryKom
#
#      founded 11.9.2024
#

# the downloaded pictures of ladybugs are named in many different ways.
# this script is used to rename them into this format:
# ladybug.X.jpg or notladybug.X.jpg, where X is the number of the picture

import os

DATA_DIR = r"C:\Users\michn\Desktop\convolutions\mixed_subsets\train\notladybug"

i = 1
for name in os.listdir(DATA_DIR):
	src = DATA_DIR + "\\" + name
	dest = DATA_DIR + fr"\notladybug.{i}.jpg"
	os.rename(src, dest)
	i += 1