import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import {
  SocialLoginModule,
  SocialAuthServiceConfig,
  GoogleLoginProvider
} from '@abacritt/angularx-social-login';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideHttpClient(),
    importProvidersFrom(SocialLoginModule),
     {
       provide: 'SocialAuthServiceConfig',
       useValue: {
         autoLogin: false,
         providers: [
           {
             id: GoogleLoginProvider.PROVIDER_ID,
             provider: new GoogleLoginProvider(
               'CHAVE AQUI'
             )
           }
         ],
         onError: (err: any) => {
           console.error(err);
         }
       } as SocialAuthServiceConfig
     }
  ]
};
