# ACT.FFXIVTranslate
A plugin to translate palyers' chatting for Final Fantasy XIV

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
2. Click the link *Get a free API key.* on that page.
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
5. Click tje link **Keys** at the left side, and you will find two keys. Any of those should be work.
