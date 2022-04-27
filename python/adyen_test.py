from ssl import ALERT_DESCRIPTION_ILLEGAL_PARAMETER
import requests
from requests.structures import CaseInsensitiveDict
import Adyen
import uuid
import json
from dotenv import load_dotenv
import os

# https://docs.adyen.com/online-payments/api-only?tab=codeBlockmethods_request_dcSnj_py_5

# to load the .env file
load_dotenv()

API_KEY = os.getenv('API_KEY')
CLIENT_KEY = os.getenv('CLIENT_KEY')
MERCHANT_ACCOUNT = os.getenv('MERCHANT_ACCOUNT')

url = "https://checkout-test.adyen.com/v68/payments"

def bidon():
    headers = CaseInsensitiveDict()
    headers["x-api-key"] = API_KEY
    headers["content-type"] = "application/json"

    data = """
    {
    "merchantAccount": \""""+MERCHANT_ACCOUNT+"""\",
    "reference": "My first Adyen test payment",
    "amount": {
        "value": 5000,
        "currency": "EUR"
    },
        "paymentMethod": {
        "type": "scheme",
        "encryptedCardNumber": "test_4111111111111111",
        "encryptedExpiryMonth": "test_03",
        "encryptedExpiryYear": "test_2030",
        "encryptedSecurityCode": "test_737"
    }
    }
    """

    resp = requests.post(url, headers=headers, data=data)

    print (resp.text)

def with_adyen(host_url):    
    adyen = Adyen.Adyen()
    adyen.payment.client.xapikey = API_KEY
    adyen.payment.client.platform = "test"
    adyen.payment.client.merchant_account = MERCHANT_ACCOUNT

    request = {}

    request['amount'] = {"value": "300", "currency": "CHF"}
    request['reference'] = f"Reference {uuid.uuid4()}"  # provide your unique payment reference
    # set redirect URL required for some payment methods
    request['returnUrl'] = f"{host_url}/redirect?shopperOrder=myRef"
    request['countryCode'] = "CH"

    try:
        result = adyen.checkout.sessions(request)
    except Exception as e:
        print('Adyen ERROR:')
        print(e)
        exit(-1)
    formatted_response = (json.loads(result.raw_response))

    for k, v in formatted_response.items():
        print(f'{k}: {v}')


# bidon()
with_adyen("http://localhost:8000")

