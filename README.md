# Разработка профессиональных приложений

В этом репозитории находятся примеры кода с лекционных/практических занятий.

|Onion Layer|Проект|Тип|Описание|
|--|--|--|--|
|Domain|[BookStore.Domain](https://github.com/appinfd/enterprise-development-samples/tree/main/BookStore.Domain)|Class library|Содержит основные сущности, инкапсулирующие бизнес-логику приложения|
|Application|[BookStore.Application](https://github.com/appinfd/enterprise-development-samples/tree/main/BookStore.Application)|Class library|Содержит службы, отвечающие за функционирование приложения|
|Application|[BookStore.Application.Contracts](https://github.com/appinfd/enterprise-development-samples/tree/main/BookStore.Application.Contracts)|Class library|Определяет контракты и абстракции для обмена данными между клиентом и хостом|
|Infractructure|[BookStore.Infractructure.EfCore](https://github.com/appinfd/enterprise-development-samples/tree/main/BookStore.Infractructure.EfCore)|Class library|Предоставляет реализацию ORM системы, отвечает за взаимодействие между приложением и базой данных|
|Presentation|[BookStore.Server](https://github.com/appinfd/enterprise-development-samples/tree/main/BookStore.Server)|ASP.NET Core Web API|Реализует HTTP-сервис для обработки запросов и предоставления данных потребителям апи|
|UI|[BookStore.Client](https://github.com/appinfd/enterprise-development-samples/tree/main/BookStore.Client)|Blazor WebAssembly Standalone App|Клиентское приложение, потребитель апи|
