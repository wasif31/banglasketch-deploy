import { ConfigServices } from './services/config/config-services';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatSliderModule } from '@angular/material/slider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatRadioModule } from '@angular/material/radio';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule } from '@angular/material/paginator';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCarouselModule, MatCarousel } from '@ngmodule/material-carousel';
import { HomeComponentsComponent } from './Components/home/home-components/home-components.component';
import { AboutUsComponentComponent } from './Components/about/about-us-component/about-us-component.component';
import { HeaderComponent } from './Components/home/header/header.component';
import { FooterComponent } from './Components/home/footer/footer.component';
import { PeopleComponent } from './Components/people/people.component';
import { NewsComponent } from './Components/news/news.component';
import { SuParaComponent } from './Components/su-para/su-para-main/su-para-main.component';
import { AdminComponent } from './Components/admin/admin.component';
import { ResourceMainComponent } from './Components/resource/resource-main/resource-main.component';
import { ResearchMainComponent } from './Components/research/research-main/research-main.component';
import { SearchComponent } from './Components/shared-components/search/search.component';
import { LoginComponent } from './Components/auth/login/login.component';
import { SignUpComponent } from './Components/auth/sign-up/sign-up.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { UserProfileComponent } from './Components/user-profile/user-profile.component';
import { SideNavbarComponent } from './Components/home/side-navbar/side-navbar.component';
import { MatCardModule } from '@angular/material/card';
import { NewsDetailComponent } from './Components/news/news-detail/news-detail.component';
import { PublicationsComponent } from './Components/publications/publications.component';
import { HttpClientModule } from '@angular/common/http';
@NgModule({
  declarations: [
    AppComponent,
    HomeComponentsComponent,
    AboutUsComponentComponent,
    HeaderComponent,
    FooterComponent,
    PeopleComponent,
    NewsComponent,
    SuParaComponent,
    AdminComponent,
    ResourceMainComponent,
    ResearchMainComponent,
    SearchComponent,
    LoginComponent,
    SignUpComponent,
    UserProfileComponent,
    SideNavbarComponent,
    NewsDetailComponent,
    PublicationsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule, FlexLayoutModule,
    FormsModule, MatToolbarModule, MatSidenavModule, MatListModule, MatCardModule,
    MatAutocompleteModule, MatButtonModule, MatSliderModule, MatCheckboxModule, MatButtonToggleModule, MatCheckboxModule,
    MatDatepickerModule, MatDialogModule, MatTableModule, MatRadioModule, MatSnackBarModule, MatPaginatorModule,
    MatTabsModule, MatFormFieldModule, MatIconModule, MatIconModule, MatInputModule, MatSelectModule, MatProgressSpinnerModule,
    MatCarouselModule.forRoot()
  ],
  providers: [ConfigServices],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
