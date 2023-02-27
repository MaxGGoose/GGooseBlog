import { BrowserRouter, Route, Routes } from "react-router-dom";
import BigBlogPost from "./BigBlogPost";
import BlogPosts from "./BlogPosts";
import NavPanel from "./NavPanel";

export default function App() {
  return (
    <BrowserRouter>
      <NavPanel />
      <div className="p-5">
        <Routes>
          <Route index element={<BlogPosts />} />
          <Route path="post" element={<BigBlogPost />} />
        </Routes>
      </div>
    </BrowserRouter>
    );
}