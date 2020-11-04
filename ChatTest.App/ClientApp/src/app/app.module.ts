import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ChatComponent } from './chat/chat.component';
import { SearchPipe } from './pipes/search.pipe';
import { ApiService } from './services/api.service';
import { UserService } from './services/user.service';
import { CookieService } from './services/cookie.service';
import { ConversationService } from './services/conversation.service';
import { MessagesService } from './services/messages.service';
import { HubService } from './services/hub.service';
import { NgbAlertModule, NgbPaginationModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ChatAddComponent } from './chat-add/chat-add.component';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    SearchPipe,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ChatComponent,
    ChatAddComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbPaginationModule,
    NgbAlertModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'chat', component: ChatComponent }
    ])
  ],
  providers: [
    ApiService,
    UserService,
    CookieService,
    ConversationService,
    MessagesService,
    HubService,
    NgbActiveModal
  ],
  entryComponents: [
    ChatAddComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
