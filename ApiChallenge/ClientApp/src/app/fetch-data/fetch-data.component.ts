import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  @Input("pageNumber") pageNumber: number;
  @Input("storyNumber") storyNumber: number;
  @Input("increment") increment: Boolean;
  public newsApiItems = {} as NewsApiItems;
  public componentName: string;
  //public error: error;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, public route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      var p = params['pageNumber'];
      if (!p)
        this.pageNumber = 0
      else
        this.pageNumber = +p;

      var s = params['storyNumber'];
      if (!s)
        this.storyNumber = 0
      else
        this.storyNumber = +s;

      var i = params['increment'];
      if (!i)
        this.increment = false;
      else
        this.increment = i;
    });


    console.info("pageNumber: " + this.pageNumber);
    console.info("storyNumber: " + this.storyNumber);
    console.info("increment: " + this.increment);

    http.get<NewsApiItems>(
      baseUrl + 'HackerNews/GetAsyncNew?pageNumber=' + this.pageNumber + '&storyNumber=' + this.storyNumber + '&increment=' + this.increment)
      .subscribe(result => {
        this.newsApiItems = result;
        console.info(this.newsApiItems);
        this.componentName = "fetch-data";
      },
        error => console.error(error)
      );
  }
}

interface NewsApiItems {
  Stories: NewsItem[],
  TotalItemsProcessed: number,
  PageNumber: number,
  message: string
}


interface NewsItem {
  id: number;
  title: string;
  by: string;
  score: string;
  url: string;
  time: string;

  //descendants: number;
  type: string;
  kids: number[];
  kidsItems: NewsItem[];
}

