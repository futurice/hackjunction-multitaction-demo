#include "MainWidget.hpp"

#include "PopupWidget.hpp"

#include <MultiWidgets/Plugins.hpp>

namespace HackJunction {

  namespace {
    const QString codeToQuery(int code) {
      if (code >= 0 && code <= 7) /* Circle marker produces a bit random codes between 0-7.. */
        return "@Heineken";
      if (code == 1591)
        return "@multitaction";
      if (code == 1595)
        return "@slush";
      if (code == 1599)
        return "@futurice";
      return "";
    }
  }


  MainWidget::MainWidget()
  {
    //Define a CSS type what we can use from stylesheets
    setCSSType("MainWidget");

    //Let's add some finger effects
    auto sparkles = MultiWidgets::createPlugin("cornerstone.sparkles");
    addChild(sparkles);
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
