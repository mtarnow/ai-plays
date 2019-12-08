from subprocess import Popen, PIPE
import shlex
import time
from datetime import datetime
import signal
import sys

process = None

def signal_handler(sig, frame):
    #process.send_signal(signal.SIGINT)
    print("completion time: " + str((datetime.now() - start_time).total_seconds()) + " s")
    print("signal handled")

def run(command):
    process = Popen(command, stdout=PIPE, shell=True)
    while True:
        line = process.stdout.readline().rstrip()
        if not line:
            break
        yield line

signal.signal(signal.SIGINT, signal_handler)
start_time = datetime.now()
if __name__ == "__main__":
    cmd = "mlagents-learn config/my_config.yaml --env=../Platformer2/Builds/5decInt/Platformer2.exe --run-id=5decInt2.0 --train --debug --no-graphics"
    args = shlex.split(cmd)
    for path in run(args):
        print(path)
    print("completion time: " + str((datetime.now() - start_time).total_seconds()) + " s")
    print("end time: " + time.asctime())

    start_time = datetime.now()
    cmd = "mlagents-learn config/my_config.yaml --env=../Platformer2/Builds/platformInfo5decInt/Platformer2.exe --run-id=platformInfo5decInt2.0 --train --debug --no-graphics"
    args = shlex.split(cmd)
    for path in run(args):
        print(path)
    print("completion time: " + str((datetime.now() - start_time).total_seconds()) + " s")
    print("end time: " + time.asctime())
