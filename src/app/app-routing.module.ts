import { UserProfileComponent } from './Components/user-profile/user-profile.component';
import { SignUpComponent } from './Components/auth/sign-up/sign-up.component';
import { LoginComponent } from './Components/auth/login/login.component';
import { SuParaComponent } from './Components/su-para/su-para-main/su-para-main.component';
import { PeopleComponent } from './Components/people/people.component';
import { HomeComponentsComponent } from './Components/home/home-components/home-components.component';
import { AboutUsComponentComponent } from './Components/about/about-us-component/about-us-component.component';
import { AdminComponent } from './Components/admin/admin.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NewsComponent } from './Components/news/news.component';
import { NewsDetailComponent } from './Components/news/news-detail/news-detail.component';
import { PublicationsComponent } from './Components/publications/publications.component';
import { ResourceMainComponent } from './Components/resource/resource-main/resource-main.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'aboutus', component: AboutUsComponentComponent },
  { path: 'home', component: HomeComponentsComponent },
  { path: 'people', component: PeopleComponent },
  { path: 'news', component: NewsComponent },
  { path: 'news-detail', component: NewsDetailComponent },
  { path: 'suPara', component: SuParaComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'resource', component: ResourceMainComponent },
  { path: 'publications', component: PublicationsComponent },
  { path: 'profile', component: UserProfileComponent },
  { path: '**', redirectTo: '/home', pathMatch: 'full' },

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  declarations: []
})
export class AppRoutingModule { }
