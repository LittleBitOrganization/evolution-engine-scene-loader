# Сервис загрузки сцен

Дает возвожность удобно загружать сцены в нужном порядке.

## Зависимости

- Сервис использвует реализацию из **Zenject**, поэтому чтобы полностью воспользоваться модулем, требуется изначально импортировать и его.

- Обязательная зависимость - [Кор модуль](https://github.com/LittleBitOrganization/evolution-engine-core)

**Импорт кор модуля:**
```json
"com.littlebitgames.coremodule": "https://github.com/LittleBitOrganization/evolution-engine-core.git"
```

## Инициализация

```
       Container.BindInterfacesAndSelfTo<CoroutineRunner>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IScenesConfig>()
                .To<ScenesConfig>()
                .FromInstance(_scenesConfig)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ISceneLoaderService>()
                .To<ZenjectSceneLoaderService>()
                .AsSingle()
                .NonLazy();
```

# Конфиги

Для работы обязательно надо создать и заполнить конфиг со сценами, куда помещаются ссылки на SceneDescription объекты

![image](https://user-images.githubusercontent.com/66946236/203389389-c35e8f2e-b121-45c3-bdde-c1aca7334196.png)

Для корректной работы модуля, должны быть заполнены все поля!!!
- BootstrapScene - первая загружаемая сцена, загружается один раз и при рестарте не выгружается.
- Loading screen scene - сцена с лоад скрином
- Environment - сцена со светом и прочим окружением
- Location - сцены основной игровой локации
- UI scene - сцена со всем UI
- Game model context - сцена с моделями игровых сущностей

![image](https://user-images.githubusercontent.com/66946236/203389917-7a959be0-24cd-47bd-9506-e2c7f50cfc60.png)



