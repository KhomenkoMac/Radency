import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { EditBookComponent } from './edit-book/edit-book.component';

import { API_BASE_URL } from './api.service';
import { HttpClientModule } from '@angular/common/http';
import { BooksListComponent } from './books-list/books-list.component';
import { NgxsModule } from '@ngxs/store';
import { BooksListState, SelectBook, SelectedBookState } from './books.actions';
import { BooksPageComponent } from './books-page/books-page.component';
import { BookItemComponent } from "./book-item/book-item.component";
import { BookDetailsComponent } from './book-details/book-details.component';

@NgModule({
    declarations: [
        AppComponent
    ],
    providers: [
        { provide: API_BASE_URL, useValue: 'https://localhost:5000' },
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        NgxsModule.forRoot([SelectedBookState, BooksListState], {
            developmentMode: false
        }),
        BooksListComponent,
        EditBookComponent,
        BooksPageComponent,
        BookItemComponent,
        BookDetailsComponent
    ]
})
export class AppModule { }
