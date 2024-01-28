
-- Drop the table if it exists
DROP TABLE IF EXISTS reddit_posts;


-- Create the RedditPosts table
CREATE TABLE reddit_posts (
    id serial PRIMARY KEY,
    post_title VARCHAR(500) NOT NULL,
    post_id TEXT NOT NULL,
    date_time_posted TIMESTAMP NOT NULL,
    upvotes INT NOT NULL,
    num_comments INT NOT NULL,
    description TEXT NOT NULL,
    url TEXT NOT NULL,
    topic_discussed VARCHAR(255),
    sentiment_score DOUBLE PRECISION,
    
    CONSTRAINT unique_post_id_constraint UNIQUE (post_id)
);