import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {

    document.addEventListener("DOMContentLoaded", function(){

      document.querySelectorAll('.sidebar .nav-link').forEach(function(element){

        element.addEventListener('click', function (e) {
          alert('hello');
          let nextEl = element.nextElementSibling;
          let parentEl  = element.parentElement;

          if(nextEl) {
            e.preventDefault();
            //let mycollapse = new bootstrap.Collapse(nextEl);

              if(nextEl.classList.contains('show')){
                nextEl.classList.add('hide');
              } else {
                nextEl.classList.remove('hide');
                // find other submenus with class=show
              }
            }

        });
      })

    });
  }

}
