#include "TweetPuller.hpp"

#include <QFile>
#include <QDomDocument>

namespace HackJunction {

  TweetPuller::TweetPuller(const QString& query) :
    m_manager(this), m_query(query)
  {
    eventAddOut("tweet-pulled");

    // Let's use Qt's NetworkAccessManager to handle the network queries
    connect(&m_manager, SIGNAL(finished(QNetworkReply*)), this, SLOT(onDataReceived(QNetworkReply*)), Qt::DirectConnection);
    connect(&m_manager, SIGNAL(sslErrors(QNetworkReply*,QList<QSslError>)), this, SLOT(onSslErrors(QNetworkReply*,QList<QSslError>)), Qt::DirectConnection);
  }

  void TweetPuller::pull() {
    sendRequest(m_query);
  }

  void TweetPuller::sendRequest(const QString& query) {

    QUrl url("https://queryfeed.net/twitter?q="+ query);

    Radiant::info("Sending request: %s", url.toString().toUtf8().data());

    QNetworkRequest request;
    request.setUrl(url);
    QNetworkReply *reply = m_manager.get(request);//send the request
    reply->setObjectName(query);//to identify which reply it is on callback

  }

  void TweetPuller::onDataReceived(QNetworkReply* reply) {
    Radiant::info("something came back from %s", reply->objectName().toUtf8().data());
    reply->deleteLater();
    if (reply->error() != QNetworkReply::NoError) {
      Radiant::error("Network reply error: %s", reply->errorString().toUtf8().data());
      return;
    }

    const QString data = reply->readAll();

    // save result to file if interested..
    const bool debug = false;
    if (debug) {
      const QString filename = reply->objectName() + ".txt";
      QFile f(filename);
      if (f.open(QIODevice::WriteOnly)) {
        f.write(data.toUtf8().data());
        f.close();
      }
    }

    // We're done. Send both the query and the data to all the registered listeners:
    Radiant::BinaryData eventData;
    eventData.writeString(m_query);
    eventData.writeString(data);
    eventSend("tweet-pulled", eventData);
  }

  void TweetPuller::onSslErrors(QNetworkReply* reply,QList<QSslError> errors)
  {
    Radiant::error("SSL error - ignoring");
    reply->ignoreSslErrors(errors);
    reply->deleteLater();
  }

  std::vector<Tweet> parseTweetsFromXml(const QString &xml)
  {
    QDomDocument doc;
    doc.setContent(xml);

    std::vector<Tweet> result;

    QDomElement docElem = doc.documentElement();
    QDomNodeList itemNodes = docElem.elementsByTagName("item");
    for (int nItem = 0; nItem < itemNodes.count(); nItem++)
    {
      Tweet tweet;

      auto itemElem = itemNodes.at(nItem).toElement();
      QDomNode itemEntries = itemElem.firstChild();
      while (!itemEntries.isNull())
      {
        QDomElement elem = itemEntries.toElement();
        QString tagName = elem.tagName();

        if (tagName == "author")
          tweet.author = elem.text();
        else if (tagName == "description")
          tweet.text = elem.text();

        itemEntries = itemEntries.nextSibling();
      }

      result.push_back(tweet);
    }
    return result;
  }

}

//This might be needed for Visual Studio, otherwise linker complains about it:
//#include "TweetPuller.moc"
