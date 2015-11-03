#ifndef TWEETPULLER_HPP
#define TWEETPULLER_HPP

#include <Valuable/Node.hpp>

#include <QObject>
#include <QNetworkAccessManager>
#include <QSslError>
#include <QNetworkReply>

namespace HackJunction {

  struct Tweet {
    QString author;
    QString text;
  };


  class TweetPuller : public QObject, public Valuable::Node {
    Q_OBJECT;

  public:
    TweetPuller(const QString& query);
    virtual ~TweetPuller() {}

    void pull();

  public slots:
    void onSslErrors(QNetworkReply* reply,QList<QSslError> errors);

    void onDataReceived(QNetworkReply* reply);

    QString getQuery() const {
      return m_query;
    }

  private:
    void sendRequest(const QString& query);

    QNetworkAccessManager m_manager;

    QString m_query;
  };

  std::vector<Tweet> parseTweetsFromXml(const QString& xml);
}

#endif // TWEETPULLER_HPP
