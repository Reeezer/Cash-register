import requests
import json
import sys
import uuid

# 'encryptedCardNumber': 'test_4111111111111111',
# 'encryptedExpiryMonth': 'test_03',
# 'encryptedExpiryYear': 'test_2030',
# 'encryptedSecurityCode': 'test_737'
data = {
    'encryptedCardNumber': 'test_4111111111111111',
    'encryptedExpiryMonth': 'test_03',
    'encryptedExpiryYear': 'test_2030',
    'encryptedSecurityCode': 'test_737',
    
    'amountValue': 30215,
    'amountCurrency': 'CHF',
    
    'reference': 'Reference ' + str(uuid.uuid4()),	
}

try:
    port = int(sys.argv[1])
except:
    port = 55000

x = requests.post(f'http://localhost:{port}/Payout', params=data)
try:
    y = json.loads(x.content)
except:
    print(x.status_code, x.content)
    exit(-1)
print('reference is:', data['reference'])
for k, v in y.items():
    print(k, v)
