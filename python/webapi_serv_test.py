import requests
import json
import sys

data = {'i': 13}

try:
    port = int(sys.argv[1])
except:
    port = 55000

x = requests.post(f'http://localhost:{port}/Payout', params=data)
y = json.loads(x.content)

for k, v in y.items():
    print(k, v)
