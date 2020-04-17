# COVID19Tracker
A cross-platform #COVID19 case tracker application that works on Android and iOS and as well as in WPF and UWP using their native XAML. Although Xamarin.Forms supports WPF and UWP, I still use the native XAML in WPF and UWP. With this reason, I could still practice WPF and UWP. Xamarin XAML and Blend XAML are somehow the same but there are differences. [https://docs.microsoft.com/en-us/xamarin/cross-platform/desktop/controls/wpf](https://docs.microsoft.com/en-us/xamarin/cross-platform/desktop/controls/wpf)
  
## DATA
API is powered by coronatracker.com which is more updated than in John Hopkins.

## Interesting stuff
**Refreshing and sorting the listview**
It's always been my practice but barely implemented that when just refreshing the list to show updated data or just sorting things. It's always better to move the items instead of clearing the list then showing the sorted items.  
  
I tried implementing this again here since our `ObservableCollection<T>` has 221 countries and we're not paging the lists. So comparing the lazy reloading (i.e. after sorting) and we use `.Clear()` and `.Add(T)` to refresh the items in the list against just moving the items `T LookupItem()` and `.Move(int index)`. The result is interesting.  

* Lazy loading took **450 to 500 +-** millisecond to complete EVERYTIME I sort the list
* Moving took only **60 to 90 +-** millisecond. That's a huge difference in performance. Codes may be a bit longer with this one plus the look up is using `.Where`. I think it may perform even more faster if I used the traditional loop.  
  
This duration will be different based on your device. But I'm already using Samsung Galaxy S8
  
## Screenshot
![](https://raw.githubusercontent.com/jaysonragasa/jaraimages/master/COVID19Tracker/2020-04-17_1054.png)  
![](https://github.com/jaysonragasa/jaraimages/blob/master/COVID19Tracker/2020-04-17_1058.png)

## STATUS
currently developing
