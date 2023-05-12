FROM node:16.17.0

WORKDIR /ClientApp

COPY . /ClientApp

RUN npm install -g @angular/cli

RUN npm install

EXPOSE 4200

ENV ENVIRONMENT dev

CMD ng serve --host 0.0.0.0
