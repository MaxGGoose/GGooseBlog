import React from 'react'
import { useState, useEffect } from 'react';
import axios from 'axios';

export default function BigBlogPost() {
    const [blogPost, setBlogPost] = useState({});

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString)
    const blogPostId = urlParams.get('id');
    
    async function fetchBlogPost() {
        axios.get(`https://localhost:7070/api/blogpost/${blogPostId}`)
            .then((response) => setBlogPost(response.data))
            .catch((error) => console.error(error.message));
    }

    useEffect(() => {
        fetchBlogPost();
    }, []);

    const options = { day: "2-digit", month: '2-digit', year: 'numeric', hour: "2-digit", minute: "2-digit" };
    const dateCreated = new Date(blogPost.created);

    return (
        <>
            <div className="pb-4">
                <h1 className="display-6">{blogPost.title}</h1>
                <p className="h6 text-muted">{dateCreated.toLocaleString("ru-RU", options)}</p>
            </div>
            <p>{blogPost.text}</p>
        </>
    );
}
