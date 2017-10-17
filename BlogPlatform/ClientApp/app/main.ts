// Entry point for JiT compilation.
declare var System: any;

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app.module';
import { enableProdMode } from '@angular/core';


// Enables Hot Module Replacement.
declare var module: any;
if (module.hot) {
    module.hot.accept();
}

console.log(process.env);

if (process.env.NODE_ENV === 'production') {
    enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule);
