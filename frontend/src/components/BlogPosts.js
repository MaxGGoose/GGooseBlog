import axios from 'axios'
import React from 'react'
import { useState, useEffect } from 'react'
import { CardGroup, Row } from 'reactstrap';
import LittleBlogPost from './LittleBlogPost';

export default function BlogPosts() {
    const [blogPosts, setBlogPosts] = useState([])
  
    async function fetchBlogPosts() {
        axios.get("https://localhost:7070/api/blogpost")
            .then((response) => setBlogPosts(response.data))
            .catch((error) => console.error(error.message));
    }

    useEffect(() => {
        fetchBlogPosts();
    }, []);
    
    return (
        <CardGroup className="mx-auto">
            <Row className="mx-auto w-100">
                {
                    Array.from(blogPosts).map(blogpost => {
                        return <LittleBlogPost
                            key={blogpost.id}
                            postId={blogpost.id}
                            title={blogpost.title}
                            text={blogpost.text}
                            created={blogpost.created}
                        />
                    })
                }
            </Row>
        </CardGroup>
    )
}
