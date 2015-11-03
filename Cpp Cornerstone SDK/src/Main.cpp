#include <MultiWidgets/Application.hpp>
#include <MultiWidgets/Widget.hpp>
#include <MultiWidgets/ImageWidget.hpp>

#include "MainWidget.hpp"

int main(int argc, char *argv[])
{
  MultiWidgets::Application app;
  if (!app.init(argc, argv))
    return -1;

  // Add style file to the application. After this we can use CSS to configure the application
  app.addStyleFilename("style.css");

  // Create our main widget, all magic is inside that
  auto w = MultiWidgets::create<HackJunction::MainWidget>();
  app.mainLayer()->addChild(w);

  //Put a background image on the background layer
  auto bg = MultiWidgets::create<MultiWidgets::ImageWidget>();
  bg->load("../Assets/Images/background-2.jpg");
  app.backgroundLayer()->addChild(bg);

  // Start the application main loop
  app.run();
  
  return 0;
}
