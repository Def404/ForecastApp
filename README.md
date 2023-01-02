# ForecastApp
## База данных
База данных была разработана на PostgreSQL. 
В ней есть 4 таблицы: Категории, Продукты, Продажи, Пользователи.
![image](https://user-images.githubusercontent.com/71017226/210221216-13c1a9fd-d9b3-4293-9d83-11d09188c87e.png)
Рисунок 1. – ER-диаграмма базы данных
## Структура приложения
В приложении три окна (window): главное (MainWindow), окно регистрации (RegistrationWindow) и окно авторизации (AuthorizationWindow). 
Также в приложении есть страницы:
*	MainPage – главная страница
*	SellPage – страница для осуществления продаж
*	ForecastPage – страница для осуществления прогноза
*	SatisticPage – страница статистики
*	SettingsPage – страницы настроек
	- 	CategorySettingsaPage – страница настройки категории
	- ProductSettingsPage – страница настройки категории
	- ProfileSettingsPage – страница информации о пользователи

Помимо этого, были добавлены модули для взаимодействия с базой данной. 
Для подключения к базе данных используется класс «PostgreSqlConnector». 

Прогнозирование спроса реализовано в классе «ForecastingMethod». В нем представлены два метода прогнозирования: Метод экстраполяции скользящей средней и Метод линейной регрессии