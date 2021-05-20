import csv
import pandas
import time
from datetime import timedelta
import os

working_dir = os.path.abspath(os.path.dirname(os.path.dirname(__file__)))

def start_time_measure(message=None):
    if message:
        print(message)
    return time.monotonic()

def end_time_measure(start_time, print_prefix=None):
    end_time = time.monotonic()
    if print_prefix:
        print(print_prefix + str(timedelta(seconds=end_time - start_time)))
    return end_time
total_start_time = start_time_measure()    

# Arrays to hold data matched
phone_users_with_ip = []
phone_users = []

device_file = working_dir + '\\data\\csv\\DevicesWithInventoryUsernameOnly.csv'

signin_file = working_dir + '\\data\\csv\\InteractiveSignIns.csv'

df = pandas.read_csv(device_file)

sf = pandas.read_csv(signin_file)

email_list = list(df['email'].str.upper())

blah = 0
for index, row in sf.iterrows():

    username = row['email'].upper()
    ip_address = str(row['ipaddress'])

    #print(username)
    #print(ip_address)

    # Break if IP Address in our domain
    if ip_address == '1234.236.747.668' or ip_address == '5088.21.471.898' or ip_address == '1277.188.20.8202':
        blah = blah + 1 
    elif username not in email_list:
        phone_users_with_ip.append(username + ',' + ip_address)
        phone_users.append(username)
        blah = blah + 1 
        #print('found')
    else: blah = blah + 1 

result_list_ip = list(dict.fromkeys(phone_users_with_ip))
result_list = list(dict.fromkeys(phone_users))

print(len(result_list_ip))
print(len(result_list))
print(blah)
for i in result_list:
    f = open(working_dir + "\\data\\output\\usersoutputpython.csv", "a")
    f.write(i + "\n")
    f.close()
for i in result_list_ip:
    f = open(working_dir + "\\data\\output\\usersoutputwithippython.csv", "a")
    f.write(i + "\n")
    f.close()

end_time_measure(total_start_time, 'Elapsed Time Pythong: ')
