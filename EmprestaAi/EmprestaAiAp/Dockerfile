# Build Angular 19.2.11
FROM node:22.12.0 

WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install -g @angular/cli@19.2.11
RUN npm install -g @angular-devkit/build-angular@19.2.11
RUN npm install
COPY . .
EXPOSE 4200
CMD ["ng", "serve", "--host", "0.0.0.0", "--poll", "2000"]