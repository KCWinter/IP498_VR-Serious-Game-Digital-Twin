import paho.mqtt.client as mqtt
import sqlite3
import json
from time import time
import pandas as pd
from datetime import datetime, timedelta
import numpy as np
import matplotlib.pyplot as plt
import xgboost as xgb
from pandas import to_datetime
from sklearn.metrics import mean_squared_error
import pandas as pd
from datetime import datetime, timedelta

"Time Series Forecasting ML Model"
# Define the start and end datetime values
start_date = datetime(2020, 1, 1, 0, 0, 0)
end_date = datetime(2023, 9, 15, 0, 0, 0)

# Create a list of datetime values at hourly intervals
datetime_values = pd.date_range(start=start_date, end=end_date, freq='H')

# Initialize a list to store light levels
light_levels = []

# Define the time interval when the light level is 75 (12:00 to 14:00)
start_time = datetime.strptime("12:00:00", "%H:%M:%S").time()
end_time = datetime.strptime("14:00:00", "%H:%M:%S").time()

# Iterate through datetime values and assign light levels
for dt in datetime_values:
    if start_time <= dt.time() <= end_time:
        light_levels.append(75)
    else:
        light_levels.append(100)

# Create a DataFrame
df = pd.DataFrame({'Datetime': datetime_values, 'Light_Level': light_levels})

df.Datetime = pd.to_datetime(df['Datetime'])


def create_features(df):
    df = df.copy()
    df['hour'] = df['Datetime'].dt.hour
    return df


df = create_features(df)

# Create Model
train = df.loc[df.Datetime < '2023-01-01']
test = df.loc[df.Datetime >= '2023-01-01']

train = create_features(train)
test = create_features(test)

FEATURES = ['hour']
TARGET = ['Light_Level']

x_train = train[FEATURES]
y_train = train[TARGET]

x_test = test[FEATURES]
y_test = test[TARGET]

reg = xgb.XGBRegressor(n_estimators=1000, early_stopping_rounds=50, learning_rate=0.001)
reg.fit(x_train, y_train, eval_set=[(x_train, y_train), (x_test, y_test)], verbose=True)

"MQTT TO SQLite"

MQTT_HOST = 'test.mosquitto.org'
MQTT_PORT = 1883
TOPIC_2 = 'KW'

DATABASE_FILE = 'C:/Users/kyran/sqlite/SeriousGame_DB.db'


def on_connect(client, userdata, flags, rc):
    client.subscribe(TOPIC_2)


def on_message(client, userdata, message):
    payload = message.payload.decode('utf-8')

    try:
        data = json.loads(payload)
        topic = message.topic

        # Assuming timestamp is in the format "YYYY-MM-DDThh:mm:ss.fffZ"
        timestamp = data['ts']
        datetime_obj = datetime.fromisoformat(timestamp)
        hour = datetime_obj.hour

        # Make a prediction using the 'hour' value
        prediction = reg.predict([[hour]])[0]  # Assuming 'reg' is your XGBoost model

        # Convert the prediction to a string
        prediction = str(prediction)

        db_conn = userdata['db_conn']
        cursor = db_conn.cursor()

        if topic == TOPIC_2:
            sql = 'INSERT INTO light_sensor (ts, br, ldr, hour, prediction) VALUES (?, ?, ?, ?, ?)'
            cursor.execute(sql, (data['ts'], data['br'], data['ldr'], hour, prediction))
            db_conn.commit()

            print(f"Message received from {topic}: {payload}, Prediction: {prediction}")

    except json.JSONDecodeError:
        print("Invalid JSON format:", payload)
    except Exception as e:
        print("Error:", e)


def main():
    db_conn = sqlite3.connect(DATABASE_FILE)
    cursor = db_conn.cursor()

    create_table2_sql = """
    CREATE TABLE IF NOT EXISTS light_sensor (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        ts TEXT NOT NULL,
        br REAL NOT NULL,
        ldr REAL NOT NULL,
        hour INTEGER NOT NULL,
        prediction REAL
    )
    """
    cursor.execute(create_table2_sql)
    cursor.close()

    client = mqtt.Client()
    client.user_data_set({'db_conn': db_conn})

    client.on_connect = on_connect
    client.on_message = on_message

    client.connect(MQTT_HOST, MQTT_PORT)
    client.loop_forever()


if __name__ == '__main__':
    main()
