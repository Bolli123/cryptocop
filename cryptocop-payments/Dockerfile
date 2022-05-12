FROM node:11.11
LABEL maintainer="Björn"

COPY . /app
WORKDIR /app
RUN npm install
ENV AMQP_RECIEVE_URL=amqp://cryptocop:pass@message-broker/

ENTRYPOINT [ "node", "index.js" ]