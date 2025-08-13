# dit.test_title_management_service

## Запуск проекта

- docker-compose  
В консоли выполнить команду `docker-compose up -d`. В данном случае в docker будет подняты сервис и база данных.


- Dockerfile
   - Собрать образ, выполнив команду `docker build -f ./DIT.Test_Title_Management_Service.Host/Dockerfile -t dit.test_title_management_service .`
   - Запустить контейнер, выполнив команду `docker run -d --name test_title_management_service -p 8080:8080 -e ConnectionStrings__PostgreSQL='' dit.test_title_management_service`, предварительно заполнив переменную окружения ConnectionStrings__PostgreSQL


API сервиса можно будет проверить по ссылке http://localhost:8080/swagger