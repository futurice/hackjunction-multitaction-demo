#ifndef POPUPWIDGET_HPP
#define POPUPWIDGET_HPP

#include "TweetPuller.hpp"

#include <MultiWidgets/ImageWidget.hpp>

namespace HackJunction {
  class PopupWidget : public MultiWidgets::ImageWidget
  {
  public:
    PopupWidget(const QString& query);
    virtual ~PopupWidget();

  private:
    void pickRandomBackground();

    void populateWithTweets(std::vector<Tweet>& tweets);

    std::shared_ptr<TweetPuller> m_tweet;
  };

  //Defines the smart pointer types:
  INTRUSIVE_PTR_TYPEDEF(PopupWidget);

}
#endif // POPUPWIDGET_HPP
