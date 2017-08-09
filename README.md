# ACT.FFXIVTranslate
这是一个用来翻译最终幻想14国际服玩家聊天内容的ACT插件。

# 如何使用
1. 从这里下载最新版的插件 https://github.com/Noisyfox/ACT.FFXIVTranslate/releases
2. 将7z压缩包内的全部文件解压并覆盖到ACT的安装目录（ACT可执行文件本体所在目录）下。
3. 打开ACT，在 功能插件 -> 插件列表 选项卡中添加并启用ACT.FFXIVTranslate.dll插件。
4. 将悬浮窗拖拽到合适的位置，大功告成！

# 翻译来源设置
目前本插件支持以下两个在线翻译API:
- Yandax.Translate
- Microsoft Translator

我为这两个翻译API都申请了免费的API key提供给大家测试或者临时使用。

插件默认使用Yandax.Translate（老毛子的翻译），你可以在**来源**下拉列表中选择不同的翻译API。

点击**使用公用密钥**按钮会自动根据当前选择的翻译API设置我提供的免费公开的API密钥，或者你可以输入自己申请的API密钥。

请记得每次更改完翻译来源设置后要点击**应用**按钮，否则除非你重新开启ACT否则新的设置都不会生效。

# 获取你自己的API密钥
我提供的免费密钥是有各种使用限制的，你可以申请属于自己的API密钥来避免遇到这些限制。

## Yandax.Translate
1. 访问 https://tech.yandex.com/translate/
2. 点击页面中的 **Get a free API key.** 链接。
3. 遵循网页的提示一步步操作，你就可以拥有属于你自己的API密钥啦。

## Microsoft Translator
1. 访问 https://portal.azure.com/
2. 注册并登录Azure门户。（中国用户请注意，请不要切换到中国区，我不知道中国区的Key能不能用）
3. 创建一个 **Translator 文本 API** 实例：
    1. 点击页面左上角的加号按钮。
    2. 搜索 **Translator 文本 API**，在搜索结果中点击该项，然后点击 **创建**。
    3. 输入所有必填项，然后点击 **创建** 按钮。记得**定价层**这一项要选**F0**不然不是免费的。
4. 创建完实例后，在Azure门户中找到这个实例并进入详情页面。
5. 点击页面左侧的 **Keys** 链接，你会看到两个KEY。这两个KEY你可以任选一个来用。

# TODO
- 加入显示开关
- 加入谷歌翻译官方API支持。
- 加入谷歌翻译非官方API支持。
- 加入http代理支持。
- 加入socks5代理支持。
- ~~在翻译窗口中显示聊天的频道。不同频道的聊天内容会用不同颜色来显示，就和游戏里一样。~~
- (不太容易) 正常显示 *定型文字*.
- 显示消息时间
- 以后想到啥再加咯 :P

-------
# ACT.FFXIVTranslate
A你ACT plugin to translate palyers' chatting for Final Fantasy XIV

# How To
1. Download the latest version of the plugin from https://github.com/Noisyfox/ACT.FFXIVTranslate/releases
2. Extract all files inside the .7z archive, put them under your ACT installation directry,
where you can find the ACT's main executable file. Replace any existing files.
3. Open ACT, add and enable ACT.FFXIVTranslate.dll in Plugins -> Plugin Listing page.
4. Drag the translation window to your prefer location and enjoy!

# Translator Provider Settings
Currently this plugin support 2 providers:
- Yandax.Translate
- Microsoft Translator

All 2 providers are bundled with a free public API key for testing / temporarily using.

The default provider is Yandax.Translate, you could change it by selecting a different one from the **Provider** combo box.

Click **Use Free Key** button to set the API key to current provider's default key or you could enter your own API key.

Remember to click **Apply** button if you changed any provider settings otherwise the change won't take effect until next time
the ACT starts.

# Obtain your own API key
The free api keys have limits on words per month. You may want to get your own personal API keys to avoid those limits.

## Yandax.Translate
1. Visit https://tech.yandex.com/translate/
2. Click the link **Get a free API key.** on that page.
3. Follow the instructions and you will get your own API key for free.

## Microsoft Translator
1. Visit https://portal.azure.com/
2. Register and / or login to Azure portal.
3. Create a **Translator Text API** instance:
   1. Click the Add icon at the top left bar.
   2. Search **Translator Text API**, click the item from search result, and click **Create**.
   3. Fill in all the request information and click **Click**. Remember to select **F0** plan for
   pricing otherwise you will be charged.
4. After creating the instance, find it in Azure protal and go to the detail page.
5. Click the link **Keys** at the left side, and you will find two keys. Any of those should be work.

# TODO
- Add switch to show / hide the overlay
- Add official Google Translate support.
- Add unofficial Google Translate support.
- Add http proxy support.
- Add socks5 proxy support.
- ~~Display chatting channel. Have different colors for different channels, just like the game does.~~
- (unlikely) Display *Auto-Translate*.
- Show message time.
- More to come :P
