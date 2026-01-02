/* publications.css */

.pub-page { 
    padding: 4rem 2rem; 
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%); /* People 페이지와 통일감 */
    min-height: 100vh;
}

.pub-container {
    max-width: 1100px;
    margin: 0 auto;
}

.pub-section { 
    margin-bottom: 4rem; 
}

/* 연도별 헤더 스타일 */
.year-title {
    font-size: 2rem;
    font-weight: 700;
    color: #2c3e50;
    margin-bottom: 1.5rem;
    border-bottom: 3px solid #007bff; /* Primary color */
    display: inline-block;
    padding-right: 2rem;
}

.pub-list { 
    display: flex; 
    flex-direction: column; 
    gap: 1.5rem; 
}

.pub-item { 
    background: #ffffff; 
    border-radius: 12px; 
    padding: 1.8rem 2rem; 
    box-shadow: 0 4px 16px rgba(0,0,0,0.06); 
    transition: transform 0.2s ease;
    border-left: 5px solid #007bff; /* 포인트 라인 */
}

.pub-item:hover {
    transform: translateX(10px); /* 살짝 옆으로 밀리는 효과 */
}

.pub-title { 
    font-size: 1.25rem; 
    font-weight: 600;
    margin: 0 0 0.5rem; 
    color: #2c3e50; 
    line-height: 1.4;
}

.pub-venue { 
    margin: 0 0 0.4rem; 
    color: #007bff; 
    font-weight: 600; 
    font-style: italic;
}

.pub-authors { 
    margin: 0 0 0.8rem; 
    color: #7f8c8d; 
    font-size: 1rem; 
}

.pub-links {
    display: flex;
    gap: 1rem;
}

.pub-links a { 
    font-size: 0.9rem; 
    color: #ffffff; 
    background: #34495e;
    padding: 0.3rem 0.8rem;
    border-radius: 5px;
    text-decoration: none; 
    transition: background 0.2s;
}

.pub-links a:hover { 
    background: #007bff;
    text-decoration: none; 
}

/* 모바일 최적화 */
@media (max-width: 768px) {
    .pub-page { padding: 2rem 1rem; }
    .pub-item { padding: 1.2rem; }
    .pub-title { font-size: 1.1rem; }
}