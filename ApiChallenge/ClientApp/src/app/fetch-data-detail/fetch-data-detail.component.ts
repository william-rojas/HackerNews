import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data-detail.component.html'
})




export class FetchDataDetailComponent {
  @Input("id") id: number;
  @Input("id") includeChildren: Boolean;
  public newsItem: NewsItem;
  public componentName: string;
  

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, public route: ActivatedRoute) {

    this.route.queryParams.subscribe(params => {
      this.id = +params['id'];
      this.includeChildren = params['includeChildren'];
    });

    http.get<NewsItem>(baseUrl + 'HackerNews/GetDetail?id=' + this.id + "&includeChildren=" + this.includeChildren).subscribe(result => {
      this.newsItem = result;
    }, error => console.error(error));
  }
}


interface NewsItem {
  id: number;
  title: string;
  by: string;
  score: string;
  url: string;
  time: string;
  text: string;
  //descendants: number;
  type: string;
  kids: number[];
  kidsItems: NewsItem[];
}
