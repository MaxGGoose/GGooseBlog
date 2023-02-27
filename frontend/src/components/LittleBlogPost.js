import React from 'react'
import { Card, CardBody, CardSubtitle, CardText, CardTitle, CardLink } from 'reactstrap';

export default function LittleBlogPost({ title, text, created, postId }) {

    const options = { day: "2-digit", month: '2-digit', year: 'numeric', hour: "2-digit", minute: "2-digit" };
    const dateCreated = new Date(created);

    return (
        <div className="p-1 w-50">
            <Card className="w-100">
                <CardBody>
                    <CardTitle tag="h5" className="text-truncate">
                        <CardLink href={"/post?id=" + postId} className="text-reset text-decoration-none stretched-link">{title}</CardLink>
                    </CardTitle>
                    <CardSubtitle tag="h6" className="mb-2 text-muted text-truncate">
                        { dateCreated.toLocaleString("ru-RU", options) }
                    </CardSubtitle>
                    <CardText className="text-truncate">
                        { text }
                    </CardText>
                </CardBody>
            </Card>
        </div>
        );
}
