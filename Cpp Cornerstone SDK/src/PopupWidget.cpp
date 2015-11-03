#include "PopupWidget.hpp"

#include <MultiWidgets/Animation.hpp>
#include <MultiWidgets/Animators.hpp>
#include <MultiWidgets/TextWidget.hpp>
#include <MultiWidgets/DocumentWidget.hpp>
#include <Nimble/Random.hpp>

#include <QDir>
#include <functional>

namespace HackJunction {
  PopupWidget::PopupWidget(const QString& searchQuery)
  {
    setCSSType("PopupWidget");

    pickRandomBackground();

    // Let's just add some widgets as children and do the UI layout in CSS
    auto image = MultiWidgets::create<MultiWidgets::ImageWidget>();
    image->setCSSId("PopupImage");
    addChild(image);

    // Let's use the logo-image also as close button, just for example's sake
    image->eventAddListener("single-tap", [this]{
      // SDK handles cleaning of the memory for this widget and all it's children,
      // once the widget has been removed the hierarchy (and no other references exist)
      this->removeFromParent();
    });
    // Disable all other interaction. Can be done in CSS also.
    image->setInputFlags(INPUT_SINGLE_TAPS);


    // Add a listener to the size-attribute of the objects.
    // When the size changes (from CSS or from code), this will re-scale the logo image to be 30%.
    // This is a generic solution (we could just hardcode the scales),
    // but this serves as an example how to add listeners to (any) attributes.
    std::function<void()> resizer = [this, image] {
      const float preferredSize = 0.3f * this->width();
      if (image->width() > 0.f) {
        float scale = preferredSize / image->width();
        image->setScale(scale);
      }
    };

    this->attribute("size")->addListener(resizer);
    image->attribute("size")->addListener(resizer);

    auto header = MultiWidgets::create<MultiWidgets::TextWidget>();
    header->setCSSId("PopupHeader");
    header->setInputFlags(INPUT_NONE);
    header->setText(searchQuery);
    addChild(header);

    // Launch TweetPuller and register to listen the events from it

    m_tweet = std::make_shared<TweetPuller>(searchQuery);
    m_tweet->eventAddListenerBd("tweet-pulled", [this](Radiant::BinaryData& data){
      // first read the data from the event
      QString query;
      data.readString(query);

      Radiant::info(query.toUtf8().data());

      QString content;
      data.readString(content);

      // Then parse the Tweets and put to UI
      std::vector<Tweet> tweets = parseTweetsFromXml(content);
      populateWithTweets(tweets);
    }, AFTER_UPDATE); // AFTER_UPDATE invokes the callback on the main thread.

    m_tweet->pull();

  }

  PopupWidget::~PopupWidget()
  {
  }

  void PopupWidget::pickRandomBackground()
  {
    bool found = false;

    QDir dir ("../Assets/Images/Speech-bubbles");
    if (dir.exists()) {
      QStringList filter;
      filter << "*.png";

      QFileInfoList imageFiles = dir.entryInfoList(filter);
      uint32_t size = imageFiles.size();

      if (size > 0) {
        auto randomIndex = Nimble::RandomUniform::instance().rand0X(size);
        auto randomFile = imageFiles.value(randomIndex);

        this->setSource(randomFile.absoluteFilePath());
        found = true;
      }
    }

    if (found) {
      setBackgroundColor("#00000000");
    }
    else {
      setBackgroundColor("#AABBCC66");
    }
  }

  void PopupWidget::populateWithTweets(std::vector<Tweet> &tweets)
  {
    //Add a widget for each tweet
    for (auto tweet: tweets) {

      auto widget = MultiWidgets::create<MultiWidgets::DocumentWidget>();
      widget->addCSSClass("TweetText");
      addChild(widget);

      widget->setText(tweet.text);

      // calculate a random location approximately from the middle of the widget
      auto bounds = this->boundingRect();
      Nimble::Vector2f center = this->size().toVector() / 2.f;
      // first translate to center, then scale smaller
      auto m = Nimble::Matrix3f::makeTranslation(center);
      bounds.transform(m);
      bounds.scale(Nimble::Vector2f(0.5f, 0.5f));

      auto randomLocation = Nimble::RandomUniform::instance().randVec2InRect(bounds);
      widget->setLocation(randomLocation);

      //animate the widget in
      widget->setScale(0.f);
      MultiWidgets::AnimatorScale::newScaler(*widget, 1.f, 2.f);
    }
  }
}
