#ifndef MAINWIDGET_HPP
#define MAINWIDGET_HPP

#include <MultiWidgets/Widget.hpp>


namespace HackJunction {

  class MainWidget : public MultiWidgets::Widget
  {
  public:
    MainWidget();
    virtual ~MainWidget();

    void markerDown(MultiTouch::Marker m, MultiWidgets::GrabManager & gm) OVERRIDE;
  };

}

#endif // MAINWIDGET_HPP
