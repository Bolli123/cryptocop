FROM python:3.8

COPY . /src
WORKDIR /src

ENV PYTHON_ENV production

RUN pip install -r requirements.txt

CMD ["python", "./app.py"]