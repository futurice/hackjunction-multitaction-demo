#include "MainWidget.hpp"

#include "PopupWidget.hpp"

namespace HackJunction {

  namespace {
    /* @TODO Update marker codes once we have them */
    enum MarkerCode {
      MultiTaction = 41,
      Slush = 42,
      Futurice = 43
    };

    const QString codeToQuery(int code) {
      if (code == MarkerCode::MultiTaction)
        return "@multitaction";
      if (code == MarkerCode::Slush)
        return "@slush";
      if (code == MarkerCode::Futurice)
        return "@futurice";
      return "";
    }
  }


  MainWidget::MainWidget()
  {
    //Define a CSS type what we can use from stylesheets
    setCSSType("MainWidget");

  }

  MainWidget::~MainWidget()
  {
  }

  void MainWidget::markerDown(MultiTouch::Marker m, MultiWidgets::GrabManager &)
  {
    // Read the marker code once placed, and spawn a popup if the code is configured

    const uint64_t code = m.code();
    Radiant::info("Marker Down: %d", code);

    QString query = codeToQuery(code);

    if (!query.isEmpty()) {

      auto popup = MultiWidgets::create<HackJunction::PopupWidget>(query);

      Nimble::Vector2f loc = m.centerLocation();
      popup->setLocation(loc);

      addChild(popup);
      popup->raiseToTop();

    }
  }

}
