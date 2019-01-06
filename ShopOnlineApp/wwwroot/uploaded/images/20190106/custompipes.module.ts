import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { CurrencyFormatPipe } from './currency-format.pipe';

@NgModule({
  declarations: [CurrencyFormatPipe],
  imports: [  ],
  exports: [ CurrencyFormatPipe ],
  providers: [],
})
export class CustomPipesModule {}