import React from 'react';
import ReactDOM from 'react-dom';
import Card from 'react-bootstrap/Card';
import 'bootstrap/dist/css/bootstrap.min.css';
import CardGroup from 'react-bootstrap/CardGroup'
import CardDeck from 'react-bootstrap/CardDeck'
import Button from 'react-bootstrap/Button'
// Put any other imports below so that CSS from your
// components takes precedence over default styles.
//import './index.css';
//import App from './App';
//import reportWebVitals from './reportWebVitals';

//ReactDOM.render(
//  <React.StrictMode>
//    <App />
//  </React.StrictMode>,
//  document.getElementById('root')
//);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
//reportWebVitals();
function formatDate(date) {
    return date.toLocaleDateString();
}


class PostRow extends React.Component {
    render() {
        const post = this.props.post;
        //const date = new Date(this.props.postTime);
        return (
            <Card>
                <Card.Img variant="top" src={`/img/${post.imageName}`} />
                <Card.Body>
                    <Card.Title>{post.title}</Card.Title>
                    <Card.Subtitle >
                        {post.postTime}
                    </Card.Subtitle>
                    <Card.Text>{post.content}</Card.Text>
                    <Button variant="outline-primary">View More</Button>{' '}
                </Card.Body>
                <Card.Footer>
                    <small className="text-muted">Last updated {formatDate(post.updTime)}</small>
                </Card.Footer>

            </Card>
            //<tr>
            //<td>{post.title}</td>
            //<td>{post.content}</td>
            //<th>{post.imageName}</th>
            //<th>{post.postTime}</th>
            //<th>{post.updTime}</th>
            //<th>{post.userId}</th>
            //</tr>


        );
    }
}


class PostTable extends React.Component {
    render() {
        const rows = [];

        this.props.posts.forEach((post) => {

            rows.push(
                <PostRow
                    post={post}
                    key={post.id}
                />
            );

        });

        return (
            <CardDeck>

                {rows}

            </CardDeck>
        );
    }
}




const Posts = [{ "title": "this 1st", "content": "content here", "imageName": "05af2e772f204d04a3130f8daa8e2c2c.jpg", "postTime": "2020-12-10", "updTime": "0001-01-01T00:00:00", "userId": "85b111fd-77b5-45d8-8987-af297763cc36", "user": null, "categoryId": 1, "category": null, "comments": null, "id": 1 },
{ "title": "this 2nd", "content": "content here", "imageName": "e915697d2c2a4bd2b57a57e9b1d097e5.jpg", "postTime": "2020-12-10", "updTime": "0001-01-01T00:00:00", "userId": "85b111fd-77b5-45d8-8987-af297763cc36", "user": null, "categoryId": 2, "category": null, "comments": null, "id": 2 },
{ "title": "this 3rd", "content": "content here", "imageName": "a6c2e532150544639762a2ad8a622125.jpg", "postTime": "2020-12-10", "updTime": "0001-01-01T00:00:00", "userId": "85b111fd-77b5-45d8-8987-af297763cc36", "user": null, "categoryId": 2, "category": null, "comments": null, "id": 3 }]

ReactDOM.render(
    <PostTable posts={Posts} />,
    document.getElementById('root')
);