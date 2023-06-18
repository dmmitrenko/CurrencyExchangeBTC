# GSES2
## Привіт! 😎 

При розгортанні контейнера , ви можете отримати доступ до документації Swagger, додавши шлях "/swagger" до URL-адреси. 
Наприклад, якщо ваш **локальний сервер (localhost)** працює на порту **8080**, ви можете відкрити документацію Swagger, перейшовши за адресою **"http://localhost:8080/swagger"**

- Для реалізації АПІ використано паттерн Mediator та Onion архітектуру. Усі емейли зберігаються в csv файл, який автоматично створюється при записі в нього першого емейлу. 

- Для отримання курсу біткоїна до гривні використовував АПІ клієнт [coingecko](https://www.coingecko.com/api/documentations/v3#/)
- Відправка емейлів здійснюється за допомогою сервісу SendGrid


### Запуск АПІ

Для правильного запуску API потрібно
- Збудувати Docker-імейдж з відповідними налаштуваннями. 
```docker
docker build -t <your-image-name> .
```
  - Запустити контейнер на основі побудованого імейджу.
```docker
docker run -d -p 8080:80 --env APIKEY=<ТУТ АПІ ключ, який я відправив у форму!> <your-image-name>
```

Залишаючи АПІ ключ у відкритому доступі мій аккаунт заблокує тому передавайте його під час виконання команди
