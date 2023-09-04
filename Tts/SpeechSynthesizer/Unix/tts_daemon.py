import os
import argparse
import time
import signal
from TTS.api import TTS
import file_extension

# Getting arguments
ap = argparse.ArgumentParser()
ap.add_argument("-m", "--model", type=str, default="tts_models/en/ljspeech/tacotron2-DDC",
	help="tts model for speech synthesizing")
ap.add_argument("-p", "--prompt", type=str, required=True,
	help="default input prompt file path")
ap.add_argument("-o", "--output", type=str, default="output.wav",
	help="default output directory")
args = vars(ap.parse_args())

def task_signal_handler(signum, frame):
    file_extension.create_path(args["output"], True)
    with open(args["prompt"], 'r') as reader:
        tts.tts_to_file(text = reader.read(), file_path = args["output"])
    # Notify parents about completion of the task
    os.kill(int(os.getppid()), signal.SIGUSR2)

# Initialization
tts = TTS(args["model"])
signal.signal(signal.SIGUSR1, task_signal_handler)

while True:
    time.sleep(2 ** 31)