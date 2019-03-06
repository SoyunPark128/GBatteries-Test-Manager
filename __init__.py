# -*- coding: utf-8 -*-
import os
import tensorflow as tf
import csv
from flask import Flask, render_template, Response



app = Flask(__name__)

@app.route('/downloadTest/')
def downloadTest():
    tf_randomData_Ampere = tf.random_uniform([25000, 1], minval=30, maxval=40, dtype=tf.float32)
    tf_randomData_Volt = tf.random_uniform([25000, 1], minval=30, maxval=50, dtype=tf.float32)
    tf_randomData_Temperature = tf.random_uniform([25000, 1], minval=18, maxval=33, dtype=tf.float32)

    sess = tf.Session()
    sess.run(tf.global_variables_initializer())
    DataAmpere = sess.run(tf_randomData_Ampere)
    DataVolt = sess.run(tf_randomData_Volt)
    DataTemperature = sess.run(tf_randomData_Temperature)
    tf_randomData_conf = tf.concat([DataAmpere, DataVolt, DataTemperature], 1)
    confData = sess.run(tf_randomData_conf)

    csvfile = open("random.csv", "w", newline="")
    csvwriter = csv.writer(csvfile)

    for row in confData:
        csvwriter.writerow(row)
    
    csvfile.close() 
    csvfile = open("random.csv", "r", newline="")
    
    csvData = csvfile.read()
    csvfile.close()
    
    response = Response(
        csvData,
        mimetype="text/csv",
        content_type='application/octet-stream',
    )
    response.headers["Content-Disposition"] = "attachment; filename=post_export_text.csv"
    csvfile.close()

    return response
    

@app.route('/')
def hello():
    portfolio_item_num = list(range(1,3))
    count = []
    path_names = ['.\static\img\item_1',
        '.\static\img\item_2']
    print(os.getcwd())
    for path_name in path_names:
        count.append(len([name for name in os.listdir(path_name) if os.path.isfile(os.path.join(path_name, name))]))
    
    Project_Name = ['DKU TIMETABLE',
    'Pomoductive']

    Project_Desc = ['Django + EC2 + Apache Web Application for Academic Information',
    'Pomoductive is UWP + MVVM Pattern + Entity Framework Windows 10 Application to improve your productivity']
    
    urls= ['http://52.89.148.181/',
    'https://github.com/SoyunPark128/Pomoductive'
                 ]
    return render_template('index.html', portfolio_item_num=portfolio_item_num, file_count=count, Project_Name=Project_Name, Project_Desc=Project_Desc )

if __name__ == '__main__':
    app.run(debug=True)
